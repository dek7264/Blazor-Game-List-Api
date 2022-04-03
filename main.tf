provider "heroku" {}

resource "heroku_pipeline" "pipeline" {
    name = "game-list-api-pipeline"
}

# Staging
resource "heroku_app" "staging" {
  name   = "game-list-api-staging"
  region = "us"
  config_vars = {
      APP_ENV = "staging"
  }
  buildpacks = [ 
      "https://buildpack-registry.s3.amazonaws.com/buildpacks/jincod/dotnetcore.tgz" 
    ]
}

resource "heroku_addon" "stagingDatabase" {
  app_id = heroku_app.staging.id
  plan   = "heroku-postgresql:hobby-dev"
}

resource "heroku_pipeline_coupling" "staging" {
  app_id   = heroku_app.staging.id
  pipeline = heroku_pipeline.pipeline.id
  stage    = "staging"
}

resource "heroku_addon" "papertrailStaging" {
  app_id = heroku_app.staging.id
  plan   = "papertrail:choklad"
}

resource "heroku_build" "build" {
  app_id = heroku_app.staging.id

  source {
      url = "https://github.com/dek7264/Blazor-Game-List-Api/archive/master.tar.gz"
  }
}


# Production
resource "heroku_app" "production" {
  name   = "game-list-api-production"
  region = "us"
  config_vars = {
      APP_ENV = "production"
  }
  buildpacks = [ 
      "https://buildpack-registry.s3.amazonaws.com/buildpacks/jincod/dotnetcore.tgz" 
    ]
}

resource "heroku_addon" "productionDatabase" {
  app_id = heroku_app.production.id
  plan   = "heroku-postgresql:hobby-dev"
}

resource "heroku_pipeline_coupling" "production" {
  app_id   = heroku_app.production.id
  pipeline = heroku_pipeline.pipeline.id
  stage    = "production"
}

resource "heroku_addon" "papertrailProduction" {
  app_id = heroku_app.production.id
  plan   = "papertrail:choklad"
}