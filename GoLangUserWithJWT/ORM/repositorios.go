package main

import "gorm.io/gorm"

type UserRepository struct {
    DB *gorm.DB
}

func (repo *UserRepository) CreateUser(user *User) error {
    return repo.DB.Create(user).Error
}

func (repo *UserRepository) GetUserByID(id uint) (*User, error) {
    var user User
    if err := repo.DB.First(&user, id).Error; err != nil {
        return nil, err
    }
    return &user, nil
}

func (repo *UserRepository) UpdateUser(user *User) error {
    return repo.DB.Save(user).Error
}

func (repo *UserRepository) DeleteUser(id uint) error {
    return repo.DB.Delete(&User{}, id).Error
}
