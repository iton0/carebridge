# Variables
UI_DIR = CareBridge-UI
API_DIR = CareBridge.API
UI_PORT = 4200
API_PORT = 5138

.PHONY: all clean run-ui run-api start

# Default target
all: start

# 1. Kill any existing processes on the specified ports
clean:
	@echo "Cleaning up ports $(UI_PORT) and $(API_PORT)..."
	@lsof -t -i:$(UI_PORT) | xargs kill -9 2>/dev/null || true
	@lsof -t -i:$(API_PORT) | xargs kill -9 2>/dev/null || true

# 2. Start Angular Frontend
run-ui:
	@echo "Starting Angular UI..."
	cd $(UI_DIR) && ng serve

# 3. Start .NET API
run-api:
	@echo "Starting .NET API..."
	cd $(API_DIR) && dotnet run

# 4. Run both simultaneously
# The '&' runs them in the background, and 'wait' ensures 
# Ctrl+C kills the make process and its children.
start: clean
	@echo "Launching CareBridge Ecosystem..."
	$(MAKE) -j 2 run-api run-ui
