using System.Collections;
using Com.AFBiyik.PopupSystem;
using Com.AFBiyik.UIComponents;
using UnityEngine;

namespace Com.AFBiyik.MatchRow.Global.Popup
{
    public class LevelsPopup : BasePopup, IRecycleViewDataSource
    {
        [SerializeField]
        private RecycleView recycleView;
        [SerializeField]
        private GameObject cellPrefab;

        public int NumberOfItems => 20;

        public float ItemHeight => 2;

        private void Awake()
        {
            recycleView.Initialize(this);
        }

        public override void OnOpened(Hashtable args)
        {
            base.OnOpened(args);


        }

        public GameObject CreateView()
        {
            return Instantiate(cellPrefab);
        }

        public GameObject SetView(GameObject view, int index)
        {
            return view;
        }
    }
}
