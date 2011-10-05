Cyclomatic Complexity

This PowerToy is used to calculate the cyclomatic complexity of functions. It illustrates the following
ReSharper features:

 * Daemon Stage
 * Highlighting
 * Option Page
 * Settings

In order to analyze the methods for cyclomatic complexity, the plug-in creates a daemon stage process
together with the associated element processor. This processor looks at C# functions only, calculating
their cyclomatic complexity recursively.

A setting value is used to determine a threshold value of cyclomatic complexity, above which the plug-in
should highlight (underline) the function that exceeds this value. This setting is editable using the
provided option page.

In addition, this sample demonstrates a solution component that causes the daemon to re-analyse code when
the threshold value changes.