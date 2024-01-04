# Unity & project path.
UNITY_VER=2021.3.9f1 #各プロジェクトに合わせてください
UNITY_PATH=/Applications/Unity/Hub/Editor/${UNITY_VER}/Unity.app/Contents/MacOS/Unity
PROJECT_PATH=$(pwd)

# Log file path.
LOG_PATH=build_ios.log

# Make log directory.
mkdir -p log

# Make build players output path.
mkdir -p Builds

# Xcode project path.
XCODEPROJ_PATH=$PROJECT_PATH/Builds

# Build iOS Xcode project.
$UNITY_PATH \
  -batchmode \
  -quit \
  -buildTarget iOS \
  -projectPath $PROJECT_PATH \
  -executeMethod AutoBuilder.BuildPlayeriOS \
  -logFile ./log/$LOG_PATH
  
if [ $? -eq 0 ]
then
  echo "Build Xcode project succeeded."
else
  echo "Build Xcode project failed."
  cat ./log/$LOG_PATH
  exit 1
fi

echo "Clean Xcode project…"
xcodebuild clean \
  -UseNewBuildSystem=NO \
  -project $XCODEPROJ_PATH/Unity-iPhone.xcodeproj

if [ $? -eq 0 ]
then
  echo "Clean Xcode project succeeded."
else
  echo "Clean Xcode project failed."
  exit 1
fi

#cocoapodsを使う場合はJenkinsのジョブ設定でBoolean設定を追加してください
#ハイカジュではcocoapodsを使うため、cocoapodsというフラグで管理しています
echo "###################"
echo "#### cocoapods ####"
echo "###################"
if "${cocoapods}"; then
  cd $XCODEPROJ_PATH
    pod init
    pod install

  if [ $? -gt 0 ]
  then
    echo "cocoapods command failed!"
    exit 1
  fi
fi

#cocoapodsを使った場合はxcworkspaceの方が安定するのでそちらを使います
if "${cocoapods}"; then
echo "Select Archive Project is xcworkspace…"
xcodebuild \
  -UseNewBuildSystem=NO \
  -workspace $XCODEPROJ_PATH/Unity-iPhone.xcworkspace \
  -scheme Unity-iPhone \
  -configuration Release ENABLE_BITCODE=NO clean build \
  -archivePath $XCODEPROJ_PATH/${REPOSITORY}.xcarchive \
  archive
else
echo "Select Archive Project is xcodeproj…"
xcodebuild \
  -UseNewBuildSystem=NO \
  -project $XCODEPROJ_PATH/Unity-iPhone.xcodeproj \
  -scheme Unity-iPhone \
  -configuration Release ENABLE_BITCODE=NO clean build \
  -archivePath $XCODEPROJ_PATH/${REPOSITORY}.xcarchive \
  archive
fi

if [ $? -eq 0 ]
then
  echo "Archive Xcode project succeeded."
else
  echo "Archive Xcode project failed."
  exit 1
fi

# AppleStore or DeployGate Upload
# AppStoreにアップするかDeployGateにアップするか選択できます
#AppStore Upload
if "${AppStore}"; then
# ipa export plist info.
TEAM_ID="VRCFZJT98K" #申請に使うアカウントのTeamID
BUNDLE_ID="com.donutsHC.${REPOSITORY}" # プロジェクトのBundleID
PROFILE_NAME="hc appstore" # 使用するプロファイル名
METHOD="app-store" # どういう形式でビルドするか。app-storeならAppStoreへアップロードします
CER="Apple Distribution" # 証明書のタイプ

# Create plist.
cat << EOF > "$XCODEPROJ_PATH/exportOptions.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>signingStyle</key>
      <string>manual</string>
    <key>teamID</key>
        <string>${TEAM_ID}</string>
    <key>signingCertificate</key>
        <string>${CER}</string>
    <key>provisioningProfiles</key>
        <dict>
          <key>${BUNDLE_ID}</key>
          <string>${PROFILE_NAME}</string>
        </dict>
      <key>method</key>
        <string>${METHOD}</string>
</dict>
</plist>
EOF

echo "Export ipa..."
xcodebuild \
  -exportArchive \
  -allowProvisioningUpdates -allowProvisioningDeviceRegistration \
  -archivePath $XCODEPROJ_PATH/${APPNAME}.xcarchive \
  -exportOptionsPlist $XCODEPROJ_PATH/exportOptions.plist \
  -exportPath $PROJECT_PATH/Builds/ipa/

if [ $? -eq 0 ]
then
  echo "Export ipa succeeded."
else
  echo "Export ipa failed."
  exit 1
fi

#AppStore Upload
echo "Upload ipa to App Store..."
xcrun altool --upload-app -f $PROJECT_PATH/Builds/ipa/${REPOSITORY}.ipa -t ios -u donuts.hc123@donuts.ne.jp -p dtro-uxtr-vliv-aioi

# DeployGate Upload
else
# ipa export plist info.
TEAM_ID="VRCFZJT98K" #申請に使わないアカウントのTeamID
BUNDLE_ID="com.donutsHC.${REPOSITORY}" # プロジェクトのBundleID
PROFILE_NAME="hc deploy" # 使用するプロファイル名
METHOD="ad-hoc" # どういう形式でビルドするか。ad-hocならipaのみ吐き出します。アカウントがEnterprizeの場合はenterprizeにしてください
CER="iOS Distribution" # 証明書のタイプ

# Create plist.
cat << EOF > "$XCODEPROJ_PATH/exportOptions.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>signingStyle</key>
      <string>manual</string>
    <key>teamID</key>
        <string>${TEAM_ID}</string>
    <key>signingCertificate</key>
        <string>${CER}</string>
    <key>provisioningProfiles</key>
        <dict>
          <key>${BUNDLE_ID}</key>
          <string>${PROFILE_NAME}</string>
        </dict>
      <key>method</key>
        <string>${METHOD}</string>
</dict>
</plist>
EOF

echo "Export ipa..."
xcodebuild \
  -exportArchive \
  -allowProvisioningUpdates -allowProvisioningDeviceRegistration \
  -archivePath $XCODEPROJ_PATH/${REPOSITORY}.xcarchive \
  -exportOptionsPlist $XCODEPROJ_PATH/exportOptions.plist \
  -exportPath $PROJECT_PATH/Builds/ipa/

if [ $? -eq 0 ]
then
  echo "Export ipa succeeded."
else
  echo "Export ipa failed."
  exit 1
fi

#DeployGate Upload
# 各プロジェクトのDeployGateに合わせてください
echo "Upload ipa to DeployGate..."
curl \
  -F "token=cca5814ec0a729cc7e704ac5191fbfaba0eeb3f4" \
  -F "file=@"$XCODEPROJ_PATH"/ipa/${APPNAME}.ipa" \
  -F "message=${NOTE}" \
  https://deploygate.com/api/users/DonutsHC-Jenkins/apps/
fi
