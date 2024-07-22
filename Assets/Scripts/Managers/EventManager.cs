using System;
using Utilities;

namespace Managers
{
    public static class EventManager
    {
        public static event Action<NotificationData> OnNotificationTrigger;
        public static event Action<TutorialData> OnTutorialTrigger;

        public static void TriggerNotification(NotificationData notificationData) =>
            OnNotificationTrigger?.Invoke(notificationData);
        public static void TriggerTutorial(TutorialData tutorialData) => OnTutorialTrigger?.Invoke(tutorialData);
    }
}
