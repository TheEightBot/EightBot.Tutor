using System;
using UIKit;
using System.Collections.Generic;

namespace EightBot.Tutor.iOS
{
	class TutorialPage : UIViewController
	{
		public PageInfo PageInfo {
			get;
			private set;
		}

		float _padding = 8;
		public float Padding {
			get {
				return _padding;
			}
			set {
				_padding = value;
			}
		}

		UIImageView _imageView;

		UILabel _pageText;

		readonly List<NSLayoutConstraint> _constraints = new List<NSLayoutConstraint> ();

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (PageInfo == null)
				return;

			_imageView.Image = 
				PageInfo?.ImagePath != null
					? UIImage.FromBundle (PageInfo.ImagePath)
					: null;

			_pageText.Text = PageInfo?.PageText ?? string.Empty;
			_pageText.TextColor = PageInfo?.PageTextColor ?? UIColor.Black;

			NSLayoutConstraint _pageTextLocation = null;
			switch (PageInfo.PageTextLocation) {
				case PageTextLocation.Bottom:
					_constraints.Add (
						_pageTextLocation =
						NSLayoutConstraint.Create (
							_pageText, NSLayoutAttribute.Bottom,
							NSLayoutRelation.Equal,
							View, NSLayoutAttribute.Bottom,
							1.0f, -Padding
						)
					);
					break;
				case PageTextLocation.Middle:
					_constraints.Add (
						_pageTextLocation =
						NSLayoutConstraint.Create (
							_pageText, NSLayoutAttribute.CenterY,
							NSLayoutRelation.Equal,
							View, NSLayoutAttribute.CenterY,
							1.0f, 0f
						)
					);
					break;
				case PageTextLocation.Top:
					_constraints.Add (
						_pageTextLocation =
						NSLayoutConstraint.Create (
							_pageText, NSLayoutAttribute.Top,
							NSLayoutRelation.Equal,
							View, NSLayoutAttribute.Top,
							1.0f, Padding
						)
					);
					break;
			}

			if (_pageTextLocation != null)
				View.AddConstraint (_pageTextLocation);
		}

		public override void WillMoveToParentViewController (UIViewController parent)
		{
			base.WillMoveToParentViewController (parent);

			if (parent != null) {
				View.BackgroundColor = PageInfo?.BackgroundColor ?? UIColor.Black;				

				_imageView = new UIImageView {
					ContentMode = UIViewContentMode.ScaleAspectFit,
					TranslatesAutoresizingMaskIntoConstraints = false,
					ClipsToBounds = false
				};
				_imageView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
				_imageView.Layer.ShadowOffset = new CoreGraphics.CGSize (0, 4);
				_imageView.Layer.ShadowOpacity = .75f;
				_imageView.Layer.ShadowRadius = 4;

				Add (_imageView);

				_constraints.Add (
					NSLayoutConstraint.Create (
						_imageView, NSLayoutAttribute.Top,
						NSLayoutRelation.Equal,
						View, NSLayoutAttribute.Top,
						1.0f, Padding
					)
				);

				_constraints.Add (
					NSLayoutConstraint.Create (
						_imageView, NSLayoutAttribute.Bottom,
						NSLayoutRelation.Equal,
						View, NSLayoutAttribute.Bottom,
						1.0f, -Padding
					)
				);

				_constraints.Add (
					NSLayoutConstraint.Create (
						_imageView, NSLayoutAttribute.Leading,
						NSLayoutRelation.Equal,
						View, NSLayoutAttribute.Leading,
						1.0f, Padding
					)
				);

				_constraints.Add (
					NSLayoutConstraint.Create (
						_imageView, NSLayoutAttribute.Trailing,
						NSLayoutRelation.Equal,
						View, NSLayoutAttribute.Trailing,
						1.0f, -Padding
					)
				);

				_pageText = new UILabel { 
					TextAlignment = UITextAlignment.Center,
					Lines = 0,
					LineBreakMode = UILineBreakMode.WordWrap,
					TranslatesAutoresizingMaskIntoConstraints = false,
				};
				Add (_pageText);

				_constraints.Add (
					NSLayoutConstraint.Create (
						_pageText, NSLayoutAttribute.Leading,
						NSLayoutRelation.Equal,
						View, NSLayoutAttribute.Leading,
						1.0f, Padding
					)
				);

				_constraints.Add (
					NSLayoutConstraint.Create (
						_pageText, NSLayoutAttribute.Trailing,
						NSLayoutRelation.Equal,
						View, NSLayoutAttribute.Trailing,
						1.0f, -Padding
					)
				);

				View.AddConstraints (_constraints.ToArray ());

				View.SetNeedsUpdateConstraints ();
				View.UpdateConstraintsIfNeeded ();
			} else {
				_imageView?.Image?.Dispose ();
				_imageView?.RemoveFromSuperview ();
				_imageView?.Dispose ();

				_pageText?.Dispose ();

				View?.RemoveConstraints (_constraints.ToArray());

				for (int i = _constraints.Count - 1; i >= 0; i--) {
					_constraints [i].Dispose ();
				}
				_constraints.Clear ();
			}
		}

		public static TutorialPage CreatePage(PageInfo page){

			var tutorialPage = new TutorialPage { PageInfo = page };

			return tutorialPage;
		}
	}
}

