# Unity & project path.
UNITY_VER=2021.3.9f1
UNITY_PATH=/Applications/Unity/Hub/Editor/${UNITY_VER}/Unity.app/Contents/MacOS/Unity
PROJECT_PATH=$(pwd)
​
# Log files path.
LOG_PATH=build_android.log
​
# Make log directory.
mkdir -p log
​
# Make build players output path.
mkdir -p Builds
​
# Build Android apk.
echo "Build Android aab..."
$UNITY_PATH \
  -batchmode \
  -quit \
  -buildTarget Android \
  -projectPath $PROJECT_PATH \
  -executeMethod AutoBuilder.BuildPlayerAndroid \
  -logFile ./log/$LOG_PATH \
​  -username $username \
  -password $password

if [ $? -eq 0 ]
then
  echo "Build Android aab succeeded."
else
  echo "Build Android aab failed."
  cat ./log/$LOG_PATH
  exit 1
fi

#DeployGate Upload
# 各プロジェクトのDeployGateに合わせてください
echo "Upload ipa to DeployGate..."
curl \
  -F "token=cca5814ec0a729cc7e704ac5191fbfaba0eeb3f4" \
  -F "file=@Builds/${APPNAME}.aab" \
  -F "message=${NOTE}" \
  https://deploygate.com/api/users/DonutsHC-Jenkins/apps/
