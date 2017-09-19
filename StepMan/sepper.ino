#include <Stepper.h>

int stepsPerRevolution = 800;//1595;  // change this to fit the number of steps per revolution
// for your motor

// initialize the stepper library on pins 8 through 11:
Stepper myStepper2(stepsPerRevolution, 9, 8);
Stepper myStepper(stepsPerRevolution, 8, 9);
int inByte = 0;  
int d = -1;
int sPerRev=0;
int simpleStep = 16;
int bufferInt[4];
void setup() {
  // set the speed at 60 rpm:
  myStepper.setSpeed(100);
  myStepper2.setSpeed(100);
  
  // initialize the serial port:
  Serial.begin(9600);
}

void SetStep(int sprSteps = 16)
{
  stepsPerRevolution = sprSteps;
}

void SetRound(int sprRound = 1595)
{  
  stepsPerRevolution = sprRound;
}

int buffer2Int()
{
  int i;
  int retVal=0;
  for (i=0;i<=d;i++)
  {
    retVal = retVal * 10 + bufferInt[i];
   /* Serial.print("d - "); Serial.println(d);
    Serial.print("i - "); Serial.println(i);
    Serial.print("buff - "); Serial.println(bufferInt[i]);
     Serial.print("ret - "); Serial.println(retVal);*/   
  }
  
  return retVal;
}

void cleanBuffer()
{
  int i;
  for (i=0;i<4;i++)   
  {
    bufferInt[i]=0;
    }
}

void loop() {
  // step one revolution  in one direction:
  SetRound();
  while (Serial.available() == 0);
  inByte = Serial.read() ;
  if (inByte == 76)
  {
  //myStepper.step(stepsPerRevolution);
  myStepper.step(buffer2Int());
  
  Serial.println(buffer2Int());
  d =-1;
  //sPerRev=0;
  //delay(2500);  
  cleanBuffer();
  delay(100);
  }
  if (inByte == 82)
  {
  //myStepper2.step(stepsPerRevolution);  
  myStepper2.step(buffer2Int());  
  
  Serial.println(buffer2Int());
  //sPerRev=0;
  d =-1;
  cleanBuffer();
  delay(100);
  }

  if (inByte == 83)
  {
    myStepper.setSpeed(buffer2Int());
    myStepper2.setSpeed(buffer2Int());
    d =-1;
    cleanBuffer();
  }

  if (inByte >=48 && inByte <= 57)
  {
      d = d+1;
      bufferInt[d] = (inByte-48);
      
      //sPerRev += (inByte-48) * d;
      
     // Serial.println(inByte);
     // Serial.println(d);
  }
  
  
  //SetStep();
  
  //delay(2500);
}
