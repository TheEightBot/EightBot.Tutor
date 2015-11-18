using System;
using UIKit;
using System.Collections.Generic;

namespace EightBot.Tutor.iOS
{
	public class Tutor : UIPageViewController
	{
		List<PageInfo> _pageInfo;

		public Tutor (List<PageInfo> pageInfo) 
			: base(
				UIPageViewControllerTransitionStyle.Scroll, 
				UIPageViewControllerNavigationOrientation.Horizontal, 
				UIPageViewControllerSpineLocation.Min
			)
		{
			_pageInfo = pageInfo;
		}

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.GetPreviousViewController = (pvc, vc) => {
				var tutorialPage = vc as TutorialPage;

				var currentIndex = _pageInfo.IndexOf(tutorialPage?.PageInfo);
				return currentIndex == 0 ? null : TutorialPage.CreatePage(_pageInfo[currentIndex - 1]);
			};

			this.GetNextViewController = (nvc, vc) => {
				var tutorialPage = vc as TutorialPage;

				var currentIndex = _pageInfo.IndexOf(tutorialPage?.PageInfo);
				return currentIndex + 1 == _pageInfo.Count ? null : TutorialPage.CreatePage(_pageInfo[currentIndex + 1]);
			};

			this.GetPresentationCount = (pvc) => _pageInfo.Count;

			this.GetPresentationIndex = (pvc) => 0;

			await SetViewControllersAsync (
				new [] { TutorialPage.CreatePage(_pageInfo[0]) }, 
				UIPageViewControllerNavigationDirection.Forward,
				false);
		}
	}
}

