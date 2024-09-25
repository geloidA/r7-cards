#!/bin/bash

# Цветовые коды
RED='\033[0;31м'
GREEN='\033[0;32м'
YELLOW='\033[0;33м'
BLUE='\033[0;34м'
NC='\033[0м' # No Color

HOST_FEEDBACK_DIR=''

# Получение аргументов командной строки

while [[ $# -gt 0 ]]; do
    key="$1"
    case $key in
        --host-feedback-dir)
            HOST_FEEDBACK_DIR="$2"
            shift # past argument
            shift # past value
            ;;
        *)
            echo -e "${RED}Неизвестный параметр: $key${NC}"
            exit 1
        ;;
    esac
done

if [ -z "$HOST_FEEDBACK_DIR" ]; then
    echo -e "${RED}Не указан путь к папке с отзывами${NC}"
    exit 2
fi

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

# Создание css стилей
npx --prefix WebUI/Cardmngr/node_modules tailwindcss -i WebUI/Cardmngr/wwwroot/css/site.css -o WebUI/Cardmngr/wwwroot/css/site.min.css --minify

# Создание папки для релиза
RELEASE_DIR="release-$VERSION"
mkdir -p "$RELEASE_DIR"
echo -e "${GREEN}Создана папка для релиза: $RELEASE_DIR${NC}"

# Экспортирование переменных
export VERSION
export HOST_FEEDBACK_DIR

# Копирование шаблона docker-compose.yml и замена переменной версии
envsubst < docker-compose.yml.build.template > docker-compose-build.yml

# Сборка Docker образов
docker-compose -f docker-compose-build.yml build
if [ $? -ne 0 ]; then
    echo -e "${RED}Failed to build Docker images${NC}"
    exit 1
fi
rm docker-compose-build.yml

# Сохранение Docker образов в tar.gz файлы и их удаление
declare -A projects
projects=(
    ["Onlyoffice/Onlyoffice.ProxyServer"]="onlyoffice-proxyserver"
    ["Cardmngr.FeedbackService"]="cardmngr-feedbackservice"
    ["WebUI/Cardmngr.Server"]="cardmngr-server"
)

for project in "${!projects[@]}"; do
    name=${projects[$project]}
    echo -e "${BLUE}Saving Docker image for $name...${NC}"
    docker save -o "$RELEASE_DIR/$name.tar.gz" "$name:$VERSION"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to save Docker image for $name${NC}"
        exit 1
    fi

    echo -e "${BLUE}Removing Docker image for $name...${NC}"
    docker rmi "$name:$VERSION"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to remove Docker image for $name${NC}"
        exit 1
    fi
done

# Копирование скрипта для развертывания
cat << 'EOF' > "$RELEASE_DIR/deploy.sh"
#!/bin/bash

# Считывание версии из имени директории
VERSION=$(basename $(pwd) | cut -d'-' -f2)

# Загрузка Docker образов
declare -A images
images=(
    ["onlyoffice-proxyserver"]="onlyoffice-proxyserver"
    ["cardmngr-feedbackservice"]="cardmngr-feedbackservice"
    ["cardmngr-server"]="cardmngr-server"
)

for image in "${!images[@]}"; do
    echo "Loading image for $image"
    docker load -i "$image.tar.gz"
    if [ $? -ne 0 ]; then
        echo -e "${RED}Failed to load Docker image for $image${NC}"
        exit 1
    fi
done

# Запуск Docker контейнеров
docker-compose up -d
if [ $? -ne 0 ]; then
    echo -e "${RED}Failed to start Docker containers${NC}"
    exit 1
fi
EOF

chmod +x "$RELEASE_DIR/deploy.sh"

# Копирование шаблона docker-compose.yml и замена переменной версии
envsubst < docker-compose.yml.deploy.template > "$RELEASE_DIR/docker-compose.yml"

echo -e "${GREEN}All Docker images have been built and saved successfully to $RELEASE_DIR.${NC}"
echo -e "${GREEN}Deploy script has been copied to $RELEASE_DIR.${NC}"
