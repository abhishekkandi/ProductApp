# Environment and Deployment Procedures

## Overview

This document provides detailed instructions for setting up the environment and deploying the application. It also notes the current state of role authorization on the APIs.

## Prerequisites

- **Docker**: Ensure Docker is installed and running on your machine.
- **Docker Compose**: Make sure Docker Compose is installed for managing multi-container Docker applications.

## Environment Setup

1. **Clone the Repository**

   Clone the repository to your local machine:

   ```bash
   git clone https://github.com/abhishekkandi/ProductApp.git
   cd ProductApp

2. **Environment Variables**

    ASPNETCORE_ENVIRONMENT=Development

3. **Docker Configuration**

    Use Docker Compose to build and run the application:

    docker-compose up --build

    This command will build the Docker image and start the application on port 5000.

## Current Limitations

- Role Authorization: Currently, there is no role-based authorization implemented on the APIs. This means that all endpoints are accessible without any role restrictions. Future updates may include role-based access control to enhance security.

- Monitoring and Logging: Integrate monitoring and logging solutions to track application performance and errors.

## MY TODO
- Add Different Dto for Create, Update
- Validate if UnitOfWork is working properly