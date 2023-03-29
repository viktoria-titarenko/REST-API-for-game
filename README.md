# REST API для игры в крестики-нолики

### Описание

Данное api прездназначено для игры в крестики нолики.

# Получение ключа

### Получить уникальный ключ для игрока и игры

POST: /Connect/getkey

Создание рандомного ключа.

Пример запроса:
```json
{
  "KeyGame": "",
  "Type": 1
}
```
Пример ответа:
```json
{
  "keyPlayer": "TX341R7DVHUQZ9V9SJO37C2HC52FK63Y",
  "keyGame": "DGSZ99NGGTNCNQ56ADLM0TS4ZLKT9O46",
  "viewType": 2
}
```
# Заполнение ячеек

### Заполняются ячейки и осуществляется проверка результата

POST: /Connect/cell

Заполнение ячеек. Проверка победных комбинаций.

Пример запроса:
```json
{
  "Value": 0,
  "KeyPlayer": "TX341R7DVHUQZ9V9SJO37C2HC52FK63Y",
  "KeyGame": "DGSZ99NGGTNCNQ56ADLM0TS4ZLKT9O46",
  "View": 2
}
```
Пример ответа:
```
{
   Keep going!
}
```

