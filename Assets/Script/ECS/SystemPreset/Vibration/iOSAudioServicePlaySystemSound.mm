#import <Foundation/Foundation.h>
#import <AudioToolBox/AudioToolBox.h>
#import <UIKit/UIKit.h>

extern "C" void _playSystemSound (int soundId)
{
    AudioServicesPlaySystemSound(soundId);
}

extern "C" void _playSystemSelection ()
{
    [[UISelectionFeedbackGenerator new] selectionChanged];
}