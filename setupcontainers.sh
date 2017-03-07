echo Start_script
docker --version
docker load -i nginx.tar
docker run -d -p 80:80 nginx
echo End_script
