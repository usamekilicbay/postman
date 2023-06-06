using Zenject;

namespace Merchant.Card
{
    public class BanditCard : CardBase
    {
        protected override void SwipeRight()
        {
            if (IsUsed)
                return;

            IsUsed = true;
            Fight();
        }

        protected override void SwipeLeft()
        {


            base.SwipeLeft();
        }

        private void Fight()
        {

        }

        public class Factory : PlaceholderFactory<ItemCard>
        {
        }
    }
}
