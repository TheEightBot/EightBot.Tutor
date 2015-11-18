using Foundation;
using UIKit;
using System.Collections.Generic;
using EightBot.Tutor.iOS;

namespace EightBot.Tutor.iOSDemo
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			// Code to start the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			var tutor = new Tutor.iOS.Tutor(
				new List<PageInfo> {
					new PageInfo { ImagePath = "1 - Death Star (Flat).png", PageText = "Ipsum", PageTextColor = UIColor.White },
					new PageInfo { ImagePath = "2 - TIE Fighter (Flat).png", BackgroundColor = UIColor.Red, PageText = "Sed nisi. Nulla quis sem at nibh elementum imperdiet.", PageTextLocation = PageTextLocation.Bottom, PageTextColor = UIColor.LightGray },
					new PageInfo { ImagePath = "3 - Fighting Spaceship (Flat).png", BackgroundColor = UIColor.Green, PageText = "Duis sagittis ipsum. Praesent mauris.", PageTextLocation = PageTextLocation.Middle, PageTextColor = UIColor.Yellow },
					new PageInfo { ImagePath = "4 - Avatar Ship (Flat).png", BackgroundColor = UIColor.Blue, PageText = "Fusce nec tellus sed augue semper porta.", PageTextLocation = PageTextLocation.Top, PageTextColor = UIColor.White },
					new PageInfo { ImagePath = "5 - Artifficial Gravity Module (Flat).png" },
					new PageInfo { BackgroundColor = UIColor.Purple, PageText = "Vestibulum sapien", PageTextLocation = PageTextLocation.Top, PageTextColor = UIColor.White }
				},
				"Finished",
				UIColor.Blue,
				UIColor.White
			);

			Window.RootViewController = tutor;

			// make the window visible
			Window.MakeKeyAndVisible ();

			return true;
		}

		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}


