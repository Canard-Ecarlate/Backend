using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CanardEcarlate.ViewsModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        private double _DefautConvert;
        public double DefautConvert { get => _DefautConvert; set => RaisePropertyChanged(ref _DefautConvert, value); }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged<T>(ref T prop, T val, [CallerMemberName] String propName = "")
        {
            if (
                (prop == null && val != null)
                ||
                (prop != null && val != null && !(prop.Equals(val))))
            {
                prop = val;
                RaisePropertyChanged(propName);
            }
        }

        private void RaisePropertyChanged(String propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public virtual void MessagingCenter_subscribe()
        {
        }

        public virtual void MessagingCenter_unsubscribe()
        {
        }
        public Assembly SvgAssembly => typeof(App).GetTypeInfo().Assembly;
        protected string AuthBackground => "CanardEcarlate.Resources.authBackground.png";
        protected string TableBackground => "CanardEcarlate.Resources.tableBackground.png";

        protected string Logo => "CanardEcarlate.Resources.logo.ico";
        protected string ReturnArrow => "CanardEcarlate.Resources.returnArrow.png";
        protected string CreatorIcon => "CanardEcarlate.Resources.creatorCrown.png";
        protected string StraightArrowLeft => "CanardEcarlate.Resources.straightArrowLeft.png";
        protected string StraightArrowRight => "CanardEcarlate.Resources.straightArrowRight.png";
        protected string ImgBgGrey => "CanardEcarlate.Resources.bgGrey.png";
        protected string Kirby => "CanardEcarlate.Resources.kirby.jpg";


        protected string ZeroCards => "CanardEcarlate.Resources.Cards.cards0.png";
        protected string OneCards => "CanardEcarlate.Resources.Cards.cards1.png";
        protected string TwoCards => "CanardEcarlate.Resources.Cards.cards2.png";
        protected string ThreeCards => "CanardEcarlate.Resources.Cards.cards3.png";
        protected string FourCards => "CanardEcarlate.Resources.Cards.cards4.png";
        protected string FiveCards => "CanardEcarlate.Resources.Cards.cards5.png";

        protected string BackCard => "CanardEcarlate.Resources.Cards.backCard.png";
        protected string GreenCard => "CanardEcarlate.Resources.Cards.greenCard.png";
        protected string YellowCard => "CanardEcarlate.Resources.Cards.yellowCard.png";
        protected string BombCard => "CanardEcarlate.Resources.Cards.bombCard.png";

        protected string BlueDuck => "CanardEcarlate.Resources.gentleDuckImage.png";
        protected string RedDuck => "CanardEcarlate.Resources.evilDuckImage.png";
        protected string BlackDuck => "CanardEcarlate.Resources.mysteryDuckImage.png";

        protected string Eye => "CanardEcarlate.Resources.eye.png";
        protected string BarredEye => "CanardEcarlate.Resources.barredEye.png";
    }
}
