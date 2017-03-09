echo Start_script

while ! [ -f /usr/bin/docker ]
do
  echo wait for docker
  sleep 5
done

sleep 5
service docker start

docker --version
docker load -i nginx.tar
docker run -d -p 80:80 nginx
echo End_script
