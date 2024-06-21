package main

import (
	"context"
	"log"
	"time"

	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"

	orderpb "pocgrpc/order"
	userpb "pocgrpc/user"
)

func main() {
	// Conectar ao serviço de usuário
	userConn, err := grpc.DialContext(context.Background(), "localhost:50051", grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("Failed to connect to user service: %v", err)
	}
	defer userConn.Close()

	userClient := userpb.NewUserServiceClient(userConn)

	// Conectar ao serviço de ordem
	orderConn, err := grpc.DialContext(context.Background(), "localhost:50052", grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("Failed to connect to order service: %v", err)
	}
	defer orderConn.Close()

	orderClient := orderpb.NewOrderServiceClient(orderConn)

	// Testar serviço de usuário
	testUserService(userClient)

	// Testar serviço de ordem
	testOrderService(orderClient)
}

func testUserService(client userpb.UserServiceClient) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	req := &userpb.UserRequest{Id: 1}
	res, err := client.GetUser(ctx, req)
	if err != nil {
		log.Fatalf("Error when calling GetUser: %v", err)
	}

	log.Printf("UserResponse: Id=%d, Name=%s", res.Id, res.Name)
}

func testOrderService(client orderpb.OrderServiceClient) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	req := &orderpb.OrderRequest{Id: 1}
	res, err := client.GetOrder(ctx, req)
	if err != nil {
		log.Fatalf("Error when calling GetOrder: %v", err)
	}

	log.Printf("OrderResponse: Id=%d, Product=%s, User.Id=%d, User.Name=%s", res.Id, res.Product, res.User.Id, res.User.Name)
}
