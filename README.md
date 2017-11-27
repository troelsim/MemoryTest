# MemoryTest
An example Xamarin Forms app to demonstrate a memory leak.

This app will eventually crash with Java.Lang.OutOfMemoryError on android, using Xamarin.Forms 2.3.4.224

## What it does
It has a main page and a child page. A timer navigates back and forth between these two pages using `PushAsync` and `PopAsync`. On the child page is a listView showing a collection of 20 images (Xamarin's `"icon.png"`, repeated 20 times).

The main and child pages are instantiated in the App constructor, so there are only one instance of each.
The `ListView` and its `ItemsSource` are instantiated in the child page constructor, so there are one of each.
The list is not updated and nothing is allocated within the app code when it's running, it only navigates back and forth.

Eventually it will crash with a stack trace like this, typically when it reaches ~200 mb memory

A java heap dump shows that a `ListViewRenderer` is instantiated each time the child page is shown, and not garbage collected.

    [MonoDroid] Java.Lang.OutOfMemoryError: Failed to allocate a 82956 byte allocation with 18400 free bytes and 17KB until OOM
    [MonoDroid]   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000c] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at Java.Interop.JniEnvironment+StaticMethods.CallStaticObjectMethod (Java.Interop.JniObjectReference type, Java.Interop.JniMethodInfo method, Java.Interop.JniArgumentValue* args) [0x00069] in <7cfbebb561c54efc9010b018c0846c7e>:0 
    [MonoDroid]   at Java.Interop.JniPeerMembers+JniStaticMethods.InvokeObjectMethod (System.String encodedMember, Java.Interop.JniArgumentValue* parameters) [0x00018] in <7cfbebb561c54efc9010b018c0846c7e>:0 
    [MonoDroid]   at Android.Graphics.BitmapFactory.DecodeResource (Android.Content.Res.Resources res, System.Int32 id) [0x00044] in <9ef29c909d7e4606a46b131129da3975>:0 
    [MonoDroid]   at Android.Graphics.BitmapFactory+<>c__DisplayClass25_0.<DecodeResourceAsync>b__0 () [0x00000] in <9ef29c909d7e4606a46b131129da3975>:0 
    [MonoDroid]   at System.Threading.Tasks.Task`1[TResult].InnerInvoke () [0x0000f] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Threading.Tasks.Task.Execute () [0x00010] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid] --- End of stack trace from previous location where exception was thrown ---
    [MonoDroid]   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000c] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess (System.Threading.Tasks.Task task) [0x0003e] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification (System.Threading.Tasks.Task task) [0x00028] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.TaskAwaiter.ValidateEnd (System.Threading.Tasks.Task task) [0x00008] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1+ConfiguredTaskAwaiter[TResult].GetResult () [0x00000] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at Xamarin.Forms.Platform.Android.FileImageSourceHandler+<LoadImageAsync>d__4.MoveNext () [0x000f3] in C:\BuildAgent\work\ca3766cfc22354a1\Xamarin.Forms.Platform.Android\Renderers\FileImageSourceHandler.cs:23 
    [MonoDroid] --- End of stack trace from previous location where exception was thrown ---
    [MonoDroid]   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000c] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess (System.Threading.Tasks.Task task) [0x0003e] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification (System.Threading.Tasks.Task task) [0x00028] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.TaskAwaiter.ValidateEnd (System.Threading.Tasks.Task task) [0x00008] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.TaskAwaiter`1[TResult].GetResult () [0x00000] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at Xamarin.Forms.Platform.Android.BaseCellView+<UpdateBitmap>d__35.MoveNext () [0x00090] in C:\BuildAgent\work\ca3766cfc22354a1\Xamarin.Forms.Platform.Android\Cells\BaseCellView.cs:203 
    [MonoDroid] --- End of stack trace from previous location where exception was thrown ---
    [MonoDroid]   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000c] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at System.Runtime.CompilerServices.AsyncMethodBuilderCore+<>c.<ThrowAsync>b__6_0 (System.Object state) [0x00000] in <896ad1d315ca4ba7b117efb8dacaedcf>:0 
    [MonoDroid]   at Android.App.SyncContext+<>c__DisplayClass2_0.<Post>b__0 () [0x00000] in <9ef29c909d7e4606a46b131129da3975>:0 
    [MonoDroid]   at Java.Lang.Thread+RunnableImplementor.Run () [0x00008] in <9ef29c909d7e4606a46b131129da3975>:0 
    [MonoDroid]   at Java.Lang.IRunnableInvoker.n_Run (System.IntPtr jnienv, System.IntPtr native__this) [0x00008] in <9ef29c909d7e4606a46b131129da3975>:0 
    [MonoDroid]   at (wrapper dynamic-method) System.Object:0699d930-47cc-4488-abf7-05d3562370c3 (intptr,intptr)
    [MonoDroid]   --- End of managed Java.Lang.OutOfMemoryError stack trace ---
    [MonoDroid] java.lang.OutOfMemoryError: Failed to allocate a 82956 byte allocation with 18400 free bytes and 17KB until OOM
    [MonoDroid] 	at dalvik.system.VMRuntime.newNonMovableArray(Native Method)
    [MonoDroid] 	at android.graphics.BitmapFactory.nativeDecodeAsset(Native Method)
    [MonoDroid] 	at android.graphics.BitmapFactory.decodeStream(BitmapFactory.java:700)
    [MonoDroid] 	at android.graphics.BitmapFactory.decodeResourceStream(BitmapFactory.java:535)
    [MonoDroid] 	at android.graphics.BitmapFactory.decodeResource(BitmapFactory.java:558)
    [MonoDroid] 	at android.graphics.BitmapFactory.decodeResource(BitmapFactory.java:588)
    [MonoDroid] 
