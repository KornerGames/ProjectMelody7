using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Zac
{

    [RequireComponent(typeof(Collider2D))]
    public class TargetDetector : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [TagSelector]
        private List<string> m_targetTags;

        [Tooltip("If set to FALSE, this will capture ALL targets within its range upon detection." +
            "If set to TRUE, disregard the value of Range.")]
        [SerializeField]
        private bool m_isLockedToFirstSingleTarget;

        [SerializeField]
        [DisableIf("m_isLockedToFirstSingleTarget")]
        private float m_detectionRange = 5f;

        [Space]

        [SerializeField]
        private bool m_isAdjustingHorizontally;

        [SerializeField]
        [ShowIf("m_isAdjustingHorizontally")]
        private SpriteRenderer m_compSpriteRenderer;

        [SerializeField]
        [ShowIf("m_isAdjustingHorizontally")]
        [Range(0f, 20f)]
        private float m_offsetHorizontal = 1f;

        [Space]

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip clipOnDetect;

        #endregion //Inspector Fields

        private ReactiveProperty<bool> m_isTargetDetected = new ReactiveProperty<bool>(false);
        private List<Collider2D> m_targets = new List<Collider2D>();

        private CompositeDisposable m_disposables = new CompositeDisposable();

        private Collider2D m_compCollider2D;
        private bool m_currentFlipX;

        #region Unity Callbacks

        private void Awake()
        {
            m_compCollider2D = GetComponent<Collider2D>();
        }

        private void OnDisable()
        {
            m_disposables.Clear();
            ClearTargets();
        }

        private void OnEnable()
        {
            InitControlledObservers();
            InitUnControlledObservers();
        }

        private void Start()
        {
            IsTargetDetected()
                .Where(isTargetDetected => isTargetDetected)
                .Where(_ => audioSource != null)
                .Where(_ => clipOnDetect != null)
                .Subscribe(_ => audioSource.PlayOneShot(clipOnDetect))
                .AddTo(this);
        }

        #endregion //Unity Callbacks

        #region Public API

        public ReactiveProperty<bool> IsTargetDetected() => m_isTargetDetected;

        public List<Collider2D> GetTargets() => m_targets;

        public Collider2D GetFirstTarget() => 
            (m_targets.Count > 0) ? m_targets[0] : null;

        public void CheckTargetsListForDestruction()
        {
            for (int x = (m_targets.Count - 1); x >= 0; x--)
            {
                if (m_targets[x] == null)
                {
                    m_targets.RemoveAt(x);
                }
            }

            if (m_targets.Count == 0)
            {
                m_isTargetDetected.Value = false;
            }
        }

        #endregion //Public API

        #region Client Impl

        private void InitControlledObservers()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
                .Subscribe(otherCollider2D => CaptureTargets(otherCollider2D))
                .AddTo(m_disposables);

            this.OnTriggerExit2DAsObservable()
                .Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
                .Subscribe(otherCollider2D => ClearTargets())
                .AddTo(m_disposables);

            this.OnCollisionEnter2DAsObservable()
                .Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
                .Subscribe(otherCollision2D => CaptureTargets(otherCollision2D.collider))
                .AddTo(m_disposables);

            this.OnCollisionExit2DAsObservable()
                .Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
                .Subscribe(otherCollider2D => ClearTargets())
                .AddTo(m_disposables);

            //self-check list of targets for null items, then reset m_isTargetDetected.Value
            Observable.Interval(System.TimeSpan.FromSeconds(1))
                .Where(_ => m_isTargetDetected.Value)
                .Subscribe(_ => CheckTargetsListForDestruction())
                .AddTo(m_disposables);
        }

        private void InitUnControlledObservers()
        {
            if (m_isAdjustingHorizontally)
            {
                m_currentFlipX = m_compSpriteRenderer.flipX;

                this.FixedUpdateAsObservable()
                .Select(_ => m_compSpriteRenderer.flipX)
                .Where(hasFlipped => (hasFlipped != m_currentFlipX))
                .Subscribe(_ => {
                    OffsetCollider();
                })
                .AddTo(m_disposables);

                OffsetCollider();
            }
        }

        private void OffsetCollider()
        {
            m_currentFlipX = m_compSpriteRenderer.flipX;
            m_compCollider2D.offset = new Vector2(
                (m_offsetHorizontal * (m_currentFlipX ? -1 : 1)),
                m_compCollider2D.offset.y);
        }

        private void CaptureTargets(Collider2D targetCollider)
        {
            if (m_isTargetDetected.Value == false)
            {
                RefreshTargets(targetCollider);
                m_isTargetDetected.Value = true;
            }
        }

        private void ClearTargets()
        {
            m_isTargetDetected.Value = false;
            m_targets.Clear();
        }

        private void RefreshTargets(Collider2D detectedCollider)
        {
            m_targets.Clear();

            if (m_isLockedToFirstSingleTarget && IsMatchingTag(detectedCollider.tag))
            {
                m_targets.Add(detectedCollider);
            }
            else
            {
                Collider2D[] tempTargets = Physics2D.OverlapCircleAll(transform.position, m_detectionRange);

                //filter targets by tags
                foreach (Collider2D collider2D in tempTargets)
                {
                    if (collider2D.isActiveAndEnabled && IsMatchingTag(collider2D.tag))
                    {
                        m_targets.Add(collider2D);
                    }
                }
            }
        }

        private bool IsMatchingTag(string tag)
        {
            return m_targetTags.Contains(tag);
        }

        #endregion //Client Impl

    }

}