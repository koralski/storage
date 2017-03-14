echo Start_script

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

if docker load -i nginx.tar
then
  echo image is loaded sucessfully
else
  echo image is loading failed
fi


if docker run -d -p 80:80 nginx
then
  echo running sucessfully
else
  echo run failed
fi

echo End_script
