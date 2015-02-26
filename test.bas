10 Clear
20 For $i = 1 To 3 Step 1
30 For $j = 1 To 3 Step 1
40 Print $i
50 Print $j
60 Next $j
70 Next $i
80 Goto 100
90 Print "Goto Failed!"
100 Let $number = 8
110 If $number == 8 Then Print "$number is 8"
120 Let $x = 0
130 Let $x = $x + 1
140 Print $x
150 If $x == 0 Then Goto 130
160 Rem This is a test
170 Print "Enter a word"
180 Input $y
190 Print $y
