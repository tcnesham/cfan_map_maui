using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CFAN.SchoolMap.ViewModels
{
    public enum Message
    {
        BalanceProjectClicked,
        MyRolesChanged,
        ReceiptsChanged,
        CurrentAreaChanged,
        ExchangeRateChanged,
        RolesChanged,
        DataLoaded
    }

    public static class Messaging
    {
        public static object CommonSender { get; set; } = new object();
        public static void Subscribe(object subscriber, Message message, Action action)
        {
            MessagingCenter.Subscribe<object>(subscriber, message.ToString(), _ => action());
        }

        public static void Subscribe(object subscriber, Message message, Action<string[]> action)
        {
            MessagingCenter.Subscribe<object, string[]>(subscriber, message.ToString(), (_,arg) => action(arg));
        }

        public static void Send(Message message)
        {
            MessagingCenter.Send(CommonSender, message.ToString());
        }

        public static void Send(Message message, params string[] args)
        {
            MessagingCenter.Send(CommonSender, message.ToString(), args);
        }
    }
}
