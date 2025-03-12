using Firebase.Analytics;

namespace BouncingBall.Analytic
{
    public class AnalyticsReporter
    {
        public void ReportLevelCompletion(string levelName)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, new Parameter("LevelName", levelName));
        }
    }
}
