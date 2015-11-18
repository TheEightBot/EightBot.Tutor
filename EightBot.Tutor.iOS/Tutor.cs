using System;
using UIKit;
using System.Collections.Generic;

namespace EightBot.Tutor.iOS
{
	public class Tutor : UIPageViewController
	{
		public event EventHandler Dismissed;

		const float HorizontalButtonPadding = 16f, VerticalButtonPadding = 66f;

		List<PageInfo> _pageInfos;

		UIButton _finalize;

		string _closeButtonTitle;
		UIColor _closeButtonBackgroundColor, _closeButtonTitleColor;

		public Tutor (
			List<PageInfo> pageInfos, 
			string closeButtonTitle, UIColor closeButtonBackgroundColor, UIColor closeButtonTitleColor) 
			: base(
				UIPageViewControllerTransitionStyle.Scroll, 
				UIPageViewControllerNavigationOrientation.Horizontal, 
				UIPageViewControllerSpineLocation.Min
			)
		{
			_pageInfos = pageInfos;

			_closeButtonTitle = closeButtonTitle;
			_closeButtonBackgroundColor = closeButtonBackgroundColor;
			_closeButtonTitleColor = closeButtonTitleColor;
		}

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_finalize = new UIButton (UIButtonType.RoundedRect);
			_finalize.BackgroundColor = _closeButtonBackgroundColor;
			_finalize.SetTitleColor (_closeButtonTitleColor, UIControlState.Normal);
			_finalize.Alpha = 0f;
			_finalize.UserInteractionEnabled = false;
			_finalize.SetTitle (_closeButtonTitle, UIControlState.Normal);
			_finalize.TranslatesAutoresizingMaskIntoConstraints = false;
			_finalize.Layer.CornerRadius = 6f;
			_finalize.TouchUpInside += _finalize_TouchUpInside;
			this.Add (_finalize);
			this.View.SendSubviewToBack (_finalize);

			View.AddConstraint (
				NSLayoutConstraint.Create (
					_finalize, NSLayoutAttribute.Leading,
					NSLayoutRelation.Equal,
					View, NSLayoutAttribute.Leading,
					1.0f, HorizontalButtonPadding
				)
			);

			View.AddConstraint (
				NSLayoutConstraint.Create (
					_finalize, NSLayoutAttribute.Trailing,
					NSLayoutRelation.Equal,
					View, NSLayoutAttribute.Trailing,
					1.0f, -HorizontalButtonPadding
				)
			);

			View.AddConstraint (
				NSLayoutConstraint.Create (
					_finalize, NSLayoutAttribute.Bottom,
					NSLayoutRelation.Equal,
					View, NSLayoutAttribute.Bottom,
					1.0f, -VerticalButtonPadding
				)
			);

			this.GetPreviousViewController = (pvc, vc) => {
				var tutorialPage = vc as TutorialPage;

				var currentIndex = _pageInfos.IndexOf(tutorialPage?.PageInfo);
				return currentIndex == 0 ? null : TutorialPage.CreatePage(_pageInfos[currentIndex - 1]);
			};

			this.GetNextViewController = (nvc, vc) => {
				var tutorialPage = vc as TutorialPage;

				var currentIndex = _pageInfos.IndexOf(tutorialPage?.PageInfo);
				var isFinalPage = currentIndex + 1 == _pageInfos.Count;

				BeginInvokeOnMainThread(() => {
					if(isFinalPage)
						UIView.Animate(.1d, 0d, UIViewAnimationOptions.CurveEaseIn, 
							() => {
								View.BringSubviewToFront(_finalize);
								_finalize.Alpha = 1f;
								_finalize.UserInteractionEnabled = true;
							},
							() => {});
//					else {
//						_finalize.Alpha = 0f;
//						_finalize.UserInteractionEnabled = false;
//						View.SendSubviewToBack (_finalize);
//					}
				});

				return isFinalPage ? null : TutorialPage.CreatePage(_pageInfos[currentIndex + 1]);
			};

			this.GetPresentationCount = (pvc) => _pageInfos.Count;

			this.GetPresentationIndex = (pvc) => 0;

			await SetViewControllersAsync (
				new [] { TutorialPage.CreatePage(_pageInfos[0]) }, 
				UIPageViewControllerNavigationDirection.Forward,
				false);
		}

		public override void ViewWillDisappear (bool animated)
		{
			_finalize.TouchUpInside -= _finalize_TouchUpInside;
			_finalize.RemoveFromSuperview ();
			_finalize.Dispose ();

			foreach (var vc in ViewControllers)
				vc.Dispose ();

			base.ViewWillDisappear (animated);
		}

		void _finalize_TouchUpInside (object sender, EventArgs e)
		{
			var dismissed = Dismissed;

			if (dismissed != null)
				dismissed (this, EventArgs.Empty);
		}
	}
}

