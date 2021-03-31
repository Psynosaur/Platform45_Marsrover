## Platform45 - Mars Rover challenge

### These are my observations and notes, they are not in any particulare order, however the code is structured in such a way that these observations make more sense.

### Built using .Net Core 3.1 CLI application template from JetBrains Rider 2020.3

### For completeness sake : https://code.google.com/archive/p/marsrovertechchallenge/

These are my assumptions and notes on the problem which I will turn into code.
- This plateau, which is curiously rectangular, must be navigated by the rovers so that their on board cameras can get a complete view of the surrounding terrain to send back to Earth.
- A rover's position is represented by a combination of an x and y co-ordinates and a letter representing one of the four cardinal compass points.
- The plateau is divided up into a grid to simplify navigation.
    - An example position might be 0, 0, N, which means the rover is in the bottom left corner and facing North.
- In order to control a rover, NASA sends a simple string of letters.
    - The possible letters are 'L', 'R' and 'M'.
    - This defines "turning" the rover
        - 'L' and 'R' makes the rover spin 90 degrees left or right respectively, without moving from its current spot.
    - This defines "moving" the rover
        - 'M' means move forward one grid point, and maintain the same heading.
- Assume that the square/coordinate directly North from (x, y) is (x, y+1).
    - THIS IS IMPORTANT !!
    - It defines what a rover does when moving up N
        - Northern movement : (x, y+1)
        - From this we can discern the following
            - Eastern movement : (x+1, y)
            - Southern movement : (x,y -1)
            - Western movement : (x-1,y)
        - We should use this is a case statement to change coordinates based on the current direction of the rover(s)

### Input:
- The first line of input is the upper-right coordinates of the plateau, the lower-left coordinates are assumed to be 0,0.
- Assumptions :
    - Therefor the range from 0 - 5 is 6 units of change
    - Our array(plateau) size can then calculated with upper right x an y coords namely urx and ury
    - lenX = 0
    - lenY = 0
        - For x in range(0, urx):
            - lenX += 1
        - For y in range(0, ury):
            - lenY += 1
    - arraySize = lenX * lenY

- The rest of the input is information pertaining to the rovers that have been deployed.
    - Each rover has two lines of input.
    - The first line gives the rover's position,
    - The second line is a series of instructions telling the rover how to explore the plateau.

- The position is made up of two integers and a letter separated by spaces, corresponding to the x and y co-ordinates and the rover's orientation.

###Each rover will be finished sequentially, which means that the second rover won't start to move until the first one has finished moving.

### Output:

The output for each rover should be its final co-ordinates and heading.

### Test Input:
#### Plateau size is a 6x6 matrix given the start coordinates of 0,0
- 5 5
  
#### Rover 1 - Message
- Position / start - Line 1
    - 1 2 N
- Instruction - Line 2
    - LMLMLMLMM
      
#### Rover 2 - Message
- Position / start - Line 1
    - 3 3 E
- Instruction - Line 2
    - MMRMMRMRRM

#### Expected Output:

1 3 N

5 1 E

#### Create function for cardinal directions: 

    N = 0° / 360°
    E = 90°
    S = 180°
    W = 270°

#### Define a rover state: I never used this :D

    setState(rover)
        // state object, only changes one attribute at a time
        state = {
               x : rover.x,
               y : rover.y,
               h : rover.h
        }

#### Define turning as a function:
        
        // NOTE --> 360° = 0° 
        turnRover(h, turn):
            If turn R:
                h = h + 90
                // when h is 360°, we set it to 0° so that we get 90°(E) when adding 90°
                if h == 360:
                    h = 0
            If turn L:
                // when h is 0°, we set it to 360° so that we get 270°(W) when subtracting 90°
                if h == 0:
                    h = 360
                h = h - 90
#### Define moving as a function: 
        
        moveRover(currentX, currentY, h):
            // integer array for our coordinates
            Switch h:
                Case N:
                    state = [currentX, currentY+1, h)
                Case E:
                    state = [currentX+1, currentY, h)
                Case S:
                    state = (currentX, currentY-1, h)
                Case W:
                    state = [currentX-1, currentY, h)

### Interpret the instructions
- R = + 90° to current direction
- L = - 90° to current direction
- M = increase position in current heading by 1
### Assumptions and observations
- Turning does not change coordinates only the rovers heading
- Moving changes rovers coordinates but not heading
- Can they go out of bounds?
    - We should not expect NASA to have rovers that go out of bounds
- If they collide?
    - do they stop?
    - explode?
    - Engineering team at NASA decided just to skip commands that where erroneous
### Ways to solve problem :
1. create a loop that executes the following commands per rover for n amounts of rovers
    - Init the rover if the init coordinates are valid

2. create another loop within that splits the bytes of the command into separate instructions characters
            
        foreach char in command:
            if char L or R:
                turnRover(h, char)
            if char M:
                moveRover(x, y, h)
    - change the state of the current rover based on the type(turning/moving) of instruction byte
    - Output the coordinates and heading of the current rover
