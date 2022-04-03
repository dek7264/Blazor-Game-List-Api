terraform {
  required_providers {
    heroku = {
      source  = "heroku/heroku"
      version = "~> 5.0"
    }
  }

  required_version = ">= 1.1"

  backend "pg" {
    
  }
}