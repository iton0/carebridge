# Variables
UI_DIR = CareBridge-UI
API_DIR = CareBridge.API
UI_PORT = 4200
API_PORT = 5138

.PHONY: all clean run-ui run-api start setup

# Default target
all: start

# Setup project: installs npm packages locally and restores nuget packages
setup:
	@echo "--- Initializing CareBridge Setup ---"
	@if [ ! -d "$(UI_DIR)" ]; then echo "Error: $(UI_DIR) not found"; exit 1; fi
	cd $(UI_DIR) && npm install
	cd $(API_DIR) && dotnet restore
	@echo "--- Setup Complete ---"

# Kill any existing processes on the specified ports
clean:
	@echo "Cleaning up ports $(UI_PORT) and $(API_PORT)..."
	@lsof -t -i:$(UI_PORT) | xargs kill -9 2>/dev/null || true
	@lsof -t -i:$(API_PORT) | xargs kill -9 2>/dev/null || true

# Start Angular Frontend
run-ui:
	@echo "Starting Angular UI..."
	cd $(UI_DIR) && npx ng serve --port $(UI_PORT)

# Start .NET API
run-api:
	@echo "Starting .NET API..."
	cd $(API_DIR) && dotnet watch run --urls="http://localhost:$(API_PORT)"

# Run both simultaneously
start: clean
	@echo "Launching CareBridge Ecosystem..."
	$(MAKE) -j 2 run-ui run-api
