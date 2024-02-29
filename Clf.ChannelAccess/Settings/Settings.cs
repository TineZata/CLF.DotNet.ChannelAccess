//
// Settings.cs
//

namespace Clf.ChannelAccess
{

  // Make this a property of the ChannelsHub ??
  // Avoid using static fields !!!!
  // TODO : XML COMMENTS !!!

  public static class Settings 
  {

    /// <summary>
    /// The default timeout period to be used in 'Async' operations that we 'await'. Nominally 5 seconds.
    /// </summary>
    
    // TODO : OPTION TO READ THIS FROM A CONFIG FILE ???

    public static readonly Clf.Common.TimeIntervalInSecs CommsTimeoutPeriod_Default = (
      new(3.0)
    ) ;

    /// <summary>
    /// The nominal timeout period to be used in 'Async' operations that we 'await', 
    /// when 'CommsTimeoutsAreEnabled' returns 'true'. The actual timout used in
    /// async operations is governed by 'CommsTimeoutPeriodInEffect'.
    /// <br/><br/>
    /// The value is initialised to <see cref="CommsTimeoutPeriod_Default"/>, but may be changed.
    /// <br/><br/>
    /// Note that if 'CommsTimeoutsAreEnabled' is returning 'false', 'CommsTimeoutPeriodInEffect'
    /// will return an effectively infinite time interval (3 hours).
    /// </summary>
    
    public static Clf.Common.TimeIntervalInSecs CommsTimeoutPeriodRequested { get ; set ; } = CommsTimeoutPeriod_Default ;

    internal static bool CommsTimeoutsAreEnabled => (
      System.Diagnostics.Debugger.IsAttached 
      ? Settings_ForLowLevelDebugging.EnableCommsTimeoutsEvenWhenDebugging
      : true
    ) ;

    public static System.TimeSpan CommsTimeoutPeriodInEffect 
    {
      get => (
        CommsTimeoutsAreEnabled
        ? CommsTimeoutPeriodRequested.AsTimeSpan
        : System.TimeSpan.FromHours(3) // Effectively infinite !! 'MaxValue' isn't permitted by the 'await' ..
      ) ;
    }

    // If the initial acquisition of the Value takes longer than this,
    // a notification of type 'ValueAcquired_AfterSuspiciouslyLongDelay' 
    // will be raised. This may be useful in tracking down problems
    // with PV's that take an unreasonable time to respond.

    public static double LongestExpectedValueAcquisitionTimeInSecs = 3.0 ;

    /// <summary>
    /// Default is false, because usually it's adequate to fetch the EnumFieldInfo (ie the names
    /// of the enum options) just once when the Value is accessed for the first time.
    /// </summary>
    
    public static bool ShouldFetchEnumInfoOnEveryEnumFieldAccess = false ;

    // NO, NOT WORTH THE COMPLICATION !!!
    // public static readonly bool AllowPutValueEvenBeforeConnectionHasBeenEstablished = false ; 

    public static bool UseWeakReferenceMessenger = false ;

    // Could move this to the ChannelsHub ???

    public static readonly CommunityToolkit.Mvvm.Messaging.IMessenger Messenger = (
      // DummyMessenger.Default
      UseWeakReferenceMessenger
      ? CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default
      : CommunityToolkit.Mvvm.Messaging.StrongReferenceMessenger.Default
    ) ;

    
    // H gives us the hour as 0..23
    // fff gives us millisecond resolution
    // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings

    public static string DefaultFormatForServerTimeStampDisplay = "H:mm:ss.fff" ;

  }

}
