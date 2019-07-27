# TaskAide
Desktop app (in active development) that enables one to track how they spend their time at work.

As someone who is efficiency-minded, I like to be able to review my performance to determine areas of improvement. For instance:
* When juggling multiple projects, with less time than work, am I allocating my time to projects proportionally to their importance?
* What factor of my time is spent engaging in less productive activities (eg. emails, poorly managed meetings)?

This first step in enabling this analysis is having a means to measure that performance.


## Project Purpose
1. Demonstrate proficiency with programming and automated testing to mitigate concerns of prospective employers.
2. Explore new tools and technologies through application of said tools and technologies.
3. Develop an app that I would utilize.

Note: the focus has been on prototyping new features, not shoring up developed ones, so execution flows and tests spotlight ideal usage.


## Utilized
* Development: C#, .NET/UWP, Visual Studio, Team Explorer (git)
* Testing: xUnit, Appium (UI Automation), Test Explorer (test runner)
* Data: SQLIte

## Showcase

![Image1](https://github.com/coltsavage/TaskAide/blob/master/Images/image1.png) 

The UI is initially designed simply for functionality so as to prioritize prototyping features.

Time control (pause/resume, speedup/slowdown, jump ahead) is enabled along the bottom so as to have more control while engaging the behavior during development, and additionally necessary to enable determinism for automated testing.

###### Tasks

Work is started once a task is selected from the drop-down.

![Image2](https://github.com/coltsavage/TaskAide/blob/master/Images/image2.png)

Tasks can be added with the Add flyout or renamed/removed through the Settings dialog (which also enables changing the color of associated intervals).

<img align="left" src="https://github.com/coltsavage/TaskAide/blob/master/Images/image3.png">

![Image4](https://github.com/coltsavage/TaskAide/blob/master/Images/image4.png)

Task persistence between sessions achieved via SQLite database.


###### Intervals

Work on tasks occurs over Intervals.  Selecting a task starts a new interval associated with that task.

![Image5](https://github.com/coltsavage/TaskAide/blob/master/Images/image5.png)

Changing the name or color of a task auto-propagates to the associated intervals.

![Image6](https://github.com/coltsavage/TaskAide/blob/master/Images/image6.png)

An intervals start time or span can be changed through dragging the front or end, respectively.

![Image7](https://github.com/coltsavage/TaskAide/blob/master/Images/image7.png)


## Future Plans
* Session persistence (eg. work days) so as to recall for later analysis.
* New UI for loading/manipulating/analyzing sessions in aggregate.
* Grouping Tasks into Projects to enable project-level telemetry.
