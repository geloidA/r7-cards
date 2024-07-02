#!/bin/bash

# Цветовые коды
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Путь к файлу с версией
VERSION_FILE="WebUI/Cardmngr.Server/appversion"

# Проверка, существует ли файл с версией
if [ ! -f "$VERSION_FILE" ]; then
    echo -e "${RED}Файл с версией не найден: $VERSION_FILE${NC}"
    exit 1
fi

# Чтение версии из файла
VERSION=$(cat "$VERSION_FILE")

# Проверка, что версия не пуста
if [ -z "$VERSION" ]; then
    echo -e "${RED}Версия не указана в файле: $VERSION_FILE${NC}"
    exit 1
fi

# Создание папки для релиза
RELEASE_DIR="release-$VERSION"
mkdir -p "$RELEASE_DIR"
echo -e "${GREEN}Создана папка для релиза: $RELEASE_DIR${NC}"

# Укажите пути к проектам и их названия
declare -A projects
projects=(
    ["Onlyoffice/Onlyoffice.ProxyServer/Onlyoffice.ProxyServer.csproj"]="onlyoffice-proxyserver"
    ["Cardmngr.FeedbackService/Cardmngr.FeedbackService.csproj"]="cardmngr-feedbackservice"
    ["WebUI/Cardmngr.Server/Cardmngr.Server.csproj"]="cardmngr-server"
)

# Выполнение команды dotnet publish для каждого проекта
for project in "${!projects[@]}"; do
    name=${projects[$project]}
    echo -e "${BLUE}Publishing $name...${NC}"
    dotnet publish $project -t:PublishContainer -r "linux-x64" -p:ContainerImageTag="$VERSION"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to publish $name${NC}"
        exit 1
    fi

    # Сохранение Docker образа и сжатие в tar.gz в папку релиза
    echo -e "${BLUE}Saving $name:$VERSION as $RELEASE_DIR/$name.tar.gz...${NC}"
    docker save "$name:$VERSION" | gzip > "$RELEASE_DIR/$name.tar.gz"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to save $name:$VERSION${NC}"
        exit 1
    fi
done

# Копирование скрипта для загрузки, тегирования и пуша
cat << 'EOF' > "$RELEASE_DIR/deploy.sh"
#!/bin/bash

# Цветовые коды
RED='\033[0;31m'
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Получение версии из названия папки
VERSION=$(basename "$(pwd)" | sed 's/release-//')

# Проверка, что версия не пуста
if [ -z "$VERSION" ]; then
    echo -e "${RED}Не удалось определить версию из названия папки${NC}"
    exit 1
fi

# Получение адреса репозитория
REPO=${1:-localhost:5000}

# Загрузка, тегирование и пуш Docker образов
for archive in *.tar.gz; do
    name=$(basename "$archive" .tar.gz)
    echo -e "${BLUE}Loading $archive...${NC}"
    docker load -i "$archive"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to load $archive${NC}"
        exit 1
    fi

    echo -e "${BLUE}Tagging $name:$VERSION as $REPO/$name:$VERSION...${NC}"
    docker tag "$name:$VERSION" "$REPO/$name:$VERSION"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to tag $name:$VERSION${NC}"
        exit 1
    fi

    echo -e "${BLUE}Pushing $REPO/$name:$VERSION...${NC}"
    docker push "$REPO/$name:$VERSION"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to push $REPO/$name:$VERSION${NC}"
        exit 1
    fi

    echo -e "${BLUE}Cleaning $name:$VERSION...${NC}"
    docker image rm "$name:$VERSION"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to clean $name:$VERSION${NC}"
        exit 1
    fi
done

echo -e "${GREEN}All Docker images have been loaded, tagged, and pushed successfully to $REPO.${NC}"
EOF

# Сделать скрипт deploy.sh исполняемым
chmod +x "$RELEASE_DIR/deploy.sh"

echo -e "${GREEN}All projects have been published and saved successfully to $RELEASE_DIR.${NC}"
echo -e "${GREEN}Deploy script has been copied to $RELEASE_DIR.${NC}"
