package main

import (
	"context"
	"log"
	"net"

	orderpb "pocgrpc/order"
	userpb "pocgrpc/user"

	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

type server struct {
	orderpb.UnimplementedOrderServiceServer
	userClient userpb.UserServiceClient
}

func (s *server) GetOrder(ctx context.Context, in *orderpb.OrderRequest) (*orderpb.OrderResponse, error) {
	userResp, err := s.userClient.GetUser(ctx, &userpb.UserRequest{Id: in.Id})
	if err != nil {
		return nil, err
	}

	return &orderpb.OrderResponse{
		Id:      in.Id,
		Product: "Product",
		User:    userResp,
	}, nil
}

func main() {
	// Conecta ao serviço de usuário
	conn, err := grpc.Dial("localhost:50051", grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("did not connect: %v", err)
	}
	defer conn.Close()

	userClient := userpb.NewUserServiceClient(conn)

	// Inicia o serviço de pedidos
	lis, err := net.Listen("tcp", ":50052")
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}
	s := grpc.NewServer()
	orderpb.RegisterOrderServiceServer(s, &server{userClient: userClient})
	log.Printf("server listening at %v", lis.Addr())
	if err := s.Serve(lis); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
}
