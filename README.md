# TaskAide
Windows (v1809) desktop app (in active development) that enables one to track how they spend their time while working.

As someone who is efficiency-minded, I like to be able to review my performance to determine areas of improvement. For instance:
- When juggling multiple projects, with less time than work, am I allocating my time to projects proportionally to their importance?
- What factor of my time is spent engaging in less productive activities (eg. emails, poorly managed meetings)?

This first step in enabling this analysis is having a means to measure that performance.


## Project Purpose
1. Demonstrate proficiency with programming and automated testing to mitigate concerns of prospective employers.
2. Explore new tools and technologies through application of those tools and technologies.
3. Develop an app that I would utilize.

Note: the focus has been on prototyping new features, not shoring up developed ones, so execution flows and tests spotlight ideal usage.


## Utilized
- Development: C#, .NET/UWP, Visual Studio, Git
- Testing: xUnit, Appium (UI Automation), Test Explorer (test runner)
- Data Storage: SQLIte

# Showcase

<img src="/Images/active_no-intervals_full.png">

The app is organized by domains:
- **Active** enables tracking of the day's time allocation.
- **Sessions** enables review of prior days' work.
- **Tasks** enables configuring tasks and observing info, metrics, and sessions where a task was worked.
- **Projects** similar to **Tasks**, but pertaining to projects (which are comprised of tasks).

At this stage in development, only Active and Tasks have implementations.

## Active

Enables tracking of time allocation for the day.

<img src="/Images/active_several-intervals_owner-popup_full.png">

Features
- **Interval Display**: observe intervals of time spent on given tasks.
  - Hovering over an interval pops-up the associated task's name.
- **Interval Adjustments**: adjust an interval's start and span by dragging the front or rear edges respectively.
- **Task Selection**: select the task for which you will be transitioning.
- **Task Addition**: add a new task to the collection.
- **Time Bar**: (development aide) control the flow of time as percieved by the app.
  - Necessary to enable determinism for automated testing and manual exploration.
  - Controls: pause/resume, speedup/slowdown, fastforward/rewind, remove last interval

Images: Interval Resizing, Task Selection, and Task Addition

<img src="/Images/active_interval-resize_partial.png" align="left">
<img src="/Images/active_task-selection-and-add-composite_partial.png">

## Tasks

At this stage of development, only enables changing Task state. Mock data is provided to simulate how Task info/metrics may be displayed in the future.

<img src="/Images/tasks_default_full.png">

Features
- **Removal**: remove tasks that no longer need tracking.
- **Rename**: rename tasks when necessary.
- **Color Customization**: change the color representing the task in the Interval Display.

Image: Result of changing a task's name and color.

<img src="/Images/active_change-name-and-color-result_full.png">
