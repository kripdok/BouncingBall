using Firebase.Analytics;

namespace BouncingBall.Analytic
{
    public class AnalyticsReporter
    {
        public async void ReportLeveCompletion(string levelName)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, new Parameter("LevelName", levelName));
        }
    }
}
