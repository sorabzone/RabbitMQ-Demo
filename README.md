# RabbitMQ-Demo
RabbitMQ Producer Consumer DotNet Core and ReactJS

# Setup RabbitMQ Cluster(3 nodes)

docker run -d --hostname rabbit1 --name myrabbit1 -p 15672:15672 -p 5672:5672 --network mynet -e RABBITMQ_ERLANG_COOKIE='rabbitcookie' rabbitmq:management

docker run -d --hostname rabbit2 --name myrabbit2 -p 5673:5672 --link myrabbit1:rabbit1 --network mynet -e RABBITMQ_ERLANG_COOKIE='rabbitcookie' rabbitmq:management

docker run -d --hostname rabbit3 --name myrabbit3 -p 5674:5672 --link myrabbit1:rabbit1 --link myrabbit2:rabbit2 --network mynet -e RABBITMQ_ERLANG_COOKIE='rabbitcookie' rabbitmq:management


After creating three nodes login to each instance and execute followng bash commands to initialize the cluster:

Node 1:

> docker exec -it myrabbit1 bash

> rabbitmqctl stop_app

> rabbitmqctl reset

> rabbitmqctl start_app


Node 2:

> docker exec -it myrabbit2 bash

> rabbitmqctl stop_app

> rabbitmqctl reset

> rabbitmqctl join_cluster --ram rabbit@rabbit1

> rabbitmqctl start_app


Node 3:

> docker exec -it myrabbit2 bash

> rabbitmqctl stop_app

> rabbitmqctl reset

> rabbitmqctl join_cluster --ram rabbit@rabbit1

> rabbitmqctl start_app

