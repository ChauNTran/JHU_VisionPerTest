2023-01-09 Update version 
in this version will add a scene to ask operator input subject Information
name, dot count dot size, dot number, background color(black/white)

2023-01-18 update
this update to add two buttons, one go up, one go down.
click button will change the color up/down direction. and still click any where 
with mouse will update the color value on the headset screen.
this is for calibration contrast.
install on the laptop and runs well.
disable reading to load image. and now other should not be used, those are not
ready yet.
2023-02-03 update 
this version is working with the dot counting. preset the random dot table. the 
table is based on the minimum dot distance. also dots are generated with grid
so the distance between two lines is the minimum dot distance.
now gislin wants the test repeat 3 times automatically. and from second time,
the dots will automatically moves to next set start with N * DotNum(preseted)
in this way no need to add another parameter, but request each dot counting test
will do 3 times to finish.
add audio to app.
add collect test results and output to C:\RPBANDRAAPLog\ with test name and subject
name combine with the test date.
2023-02-09 update to 
in this version add eye tracking data
2023-02-10 note to record audio, the purpose is to record subject's report counting and reading result.
need to make file save for that.
v9.2 eye tracking is working
need to check the format and information.
2023-02-15 update 
display image on operator screen same as image on viewer screen, works
add mouse click and draw a line on the click position on operator screen, 
this way as a mark subject has read the word correctly. 
left key green = correct. right key red = wrong or not 

2023-03-03 Update 
in this version, it is good to make mark on any of the word as mouse pointed.
if mouse left key clicked, it will mark as red color line and 30 degree tilted.
this is used to mark the word subject missed read it.
if mouse right key clicked, it will mark as blue color line and 30 degree tilted.
this is used to mark the word subject reading wroing.
if click the spacebar on the keyboard, it will remove the line draw previously. 
It will only remove one line, can not keep remove. this is use to correct if by accidently
mark a wrong word mistake.
if click the "S" key, it will make a screen shot and save to the C:/ dirctory as a reading 
test result copy, this image will include the control panel as well. so the missing word 
number and wrong word number is listed on the panel.

2023-03-08 update 
collect the miss-reading count and wrong-reading count number and display on the screen.
if one some of the mark was not right, need to correct it immediately. and will remove 
from the count number. this part is working well.
now press space-bar will remove the mark just draw. but can not go further draw.
(only delete current drawing).
press the 'S' key will make a screen copy and save to the log direcory. with current subject name

2023-04-11 Update 
In this version will use the new version of generated random dots list,
will import all the different files into tables. from grid=20 to grid = 80,
the role is:
grid = 80 , max number = 9 per test
grid = 70 , max number = 10 per test
grid = 60 , max number = 10 per test
grid = 50 , max number = 14 per test
grid = 40 , max number = 18 per test
grid = 30 , max number = 23 per test
grid = 20 , max number = 35 per test

so when select the distance in info page, will just load the table contains data
then each time select rnd number from the table by jump to location + grid number

2023-04-17 Update
in this version, make it simple: select dot number in information page, if:
Num < 9 Min= 80
9 < num < 12 Min = 60;
12 < num < 18 Min = 40;
18 < num < 35 Min = 20
this way will automatically determine the dot min distance, no need to select.
so only need to load three rnd dot files. and save as list. 
in the dot counting part, get the loaded rnd dot list to pick up the number of dots
and display in the hmd. repeat three times will just simply jump to next set of dots.

2023-04-21 update
This version both DotCounting and Reading are working well. 
out put data file is correct format and data.
will automatically select the minimum dot distance as select dots number.
this version will clean up the code and no debug info.