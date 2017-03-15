echo Start_script
imageName=$1
imageFile=$2
if [ -f `pwd`/$imageFile ]
then
  echo image download sucessfully to `pwd`/$imageFile
else
  echo image download failed
fi

while ! [ -f /usr/bin/docker ]
do
  sleep 5
done

echo docker installed

while ! docker ps  >/dev/null 2>&1
do
  sleep 5
done

echo docker is running
docker --version

if docker load -i `pwd`/$imageFile
then
  echo image is loaded sucessfully
else
  echo image loading failed
  echo "current folder is: `pwd`"
fi

if docker run -d -p 80:80 $imageName
then
  echo running sucessfully
else
  echo run failed
fi

echo End_script
