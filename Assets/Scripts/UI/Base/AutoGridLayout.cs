using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base
{
    public class AutoGridLayout : GridLayoutGroup
    {
        [SerializeField] private bool staySquare;
        [SerializeField] private bool updateOnFristFrame;
        [SerializeField] private bool fitToParent;
        protected override void Awake()
        {
            base.Awake();
            if (updateOnFristFrame)
                Observable.NextFrame().Subscribe(_ => AutoCellSize()).AddTo(this);
        }
        public void AutoCellSize()
        {
            if (m_Constraint == Constraint.Flexible) return;

            cellSize = GetCellSize();

             /*Debug.Log($"AutoCellSize: {gameObject.name} " +
                 $"w={rectTransform.rect.width} " +
                 $"par={(rectTransform.parent as RectTransform).rect.width} " +
                 $"cons={constraint} " +
                 $"count={constraintCount}");*/
        }
        private Vector2 GetCellSize()
        {
            var parenRectTransform = rectTransform.parent as RectTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(parenRectTransform);

            var rect = fitToParent ? parenRectTransform.rect : rectTransform.rect;
            var constr = GetConstrainsCount();
            var w = (rect.width - spacing.x * (constr.x - 1) - padding.left - padding.right) / constr.x;
            var h = (rect.height - spacing.y * (constr.y - 1) - padding.top - padding.bottom) / constr.y;
            if (staySquare)
            {
                if (m_Constraint == Constraint.FixedColumnCount)
                    h = w;
                else
                    w = h;

            }
            return new Vector2(w, h);
        }
        private (int x, int y) GetConstrainsCount()
        {
            (int x, int y) result;
            if (m_Constraint == Constraint.FixedColumnCount)
            {
                result.x = constraintCount;
                result.y = Mathf.CeilToInt(rectChildren.Count / (float)constraintCount);
            }
            else
            {
                result.x = Mathf.CeilToInt(rectChildren.Count / (float)constraintCount);
                result.y = constraintCount;
            }
            return result;
        }
    }
}