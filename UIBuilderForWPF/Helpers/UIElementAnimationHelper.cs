using UIBuilderForWPF.Utils;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Timers;
using System.Windows.Threading;

namespace UIBuilderForWPF.Helpers
{
    public class UIElementAnimationHelper
    {
        private static UIElementAnimationHelper Instance { get; set; } = null;
        private const string TRANSFORM_NAME = "AnimatedTranslateTransform";

        public event EventHandler AnimationCompleted ;

        private ContentControl rootContext;
        public UIElementAnimationHelper(ContentControl rootContext)
        {
            this.rootContext = rootContext;
        }

        public void RegisterTransform(UIElement animationObject)
        {
            try
            {
                TranslateTransform animatedTranslateTransform = new TranslateTransform();
                rootContext.RegisterName(TRANSFORM_NAME, animatedTranslateTransform);
                animationObject.RenderTransform = animatedTranslateTransform;
            }
            catch(Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.ToString());
                throw;
            }
        }

        public void AddElementToCollectionWithAnimation(Point startingObjectPosition, Point endingObjectPosition)
        {
            // Create the animation path.
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();
            pFigure.StartPoint = startingObjectPosition;
            LineSegment pBezierSegment = new LineSegment();
            pBezierSegment.Point = endingObjectPosition;
            pFigure.Segments.Add(pBezierSegment);
            animationPath.Figures.Add(pFigure);

            // Freeze the PathGeometry for performance benefits.
            animationPath.Freeze();

            DoubleAnimationUsingPath translateXAnimation = new DoubleAnimationUsingPath();
            translateXAnimation.PathGeometry = animationPath;
            translateXAnimation.Duration = TimeSpan.FromSeconds(Constants.ADD_ANIMATION_DURATION_SECONDS);

            // Set the Source property to X. This makes
            // the animation generate horizontal offset values from
            // the path information.
            translateXAnimation.Source = PathAnimationSource.X;

            // Set the animation to target the X property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateXAnimation, TRANSFORM_NAME);
            Storyboard.SetTargetProperty(translateXAnimation,
                new PropertyPath(TranslateTransform.XProperty));

            // Create a DoubleAnimationUsingPath to move the
            // rectangle vertically along the path by animating
            // its TranslateTransform.
            DoubleAnimationUsingPath translateYAnimation =
                new DoubleAnimationUsingPath();
            translateYAnimation.PathGeometry = animationPath;
            translateYAnimation.Duration = TimeSpan.FromSeconds(Constants.ADD_ANIMATION_DURATION_SECONDS);

            // Set the Source property to Y. This makes
            // the animation generate vertical offset values from
            // the path information.
            translateYAnimation.Source = PathAnimationSource.Y;

            // Set the animation to target the Y property
            // of the TranslateTransform named "AnimatedTranslateTransform".
            Storyboard.SetTargetName(translateYAnimation, TRANSFORM_NAME);
            Storyboard.SetTargetProperty(translateYAnimation,
                new PropertyPath(TranslateTransform.YProperty));

            // Create a Storyboard to contain and apply the animations.
            Storyboard pathAnimationStoryboard = new Storyboard();
            //pathAnimationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
            pathAnimationStoryboard.Children.Add(translateXAnimation);
            pathAnimationStoryboard.Children.Add(translateYAnimation);
            pathAnimationStoryboard.Duration = TimeSpan.FromSeconds(Constants.ADD_ANIMATION_DURATION_SECONDS);
            pathAnimationStoryboard.Begin(rootContext);

            Timer completionTimer = new Timer(Constants.ADD_ANIMATION_DURATION_SECONDS * 1000);
            completionTimer.AutoReset = false;
            completionTimer.Enabled = true;
            completionTimer.Elapsed += Animation_Completed;
        }

        private void Animation_Completed(object? sender, ElapsedEventArgs e)
        {
            rootContext.Dispatcher.BeginInvoke((Action)(() => {
                rootContext.UnregisterName(TRANSFORM_NAME);
            }));
            
            AnimationCompleted?.Invoke(sender, e);
        }

    }
}
