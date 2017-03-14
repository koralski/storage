echo Start_script

while ! [ -f /usr/bin/docker ]
do
  echo wait for docker installation
  sleep 5
done

while ! docker ps
do
  echo wait for docker service to run
  sleep 5
done

docker --version
docker load -i nginx.tar
docker run -d -p 80:80 nginx
echo End_script
