PROJECT=HelloWorld
UE4=`pwd`/../UnrealEngine
cd $UE4/UnrealEngine
./Engine/Build/BatchFiles/Mac/Build.sh ${PROJECT}Editor Mac Development $PROJECT.uproject
