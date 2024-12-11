## O Projeto

### Descrição

Arquivos de log podem revelar muito sobre o comportamento de um sistema em um ambiente de produção. A extração de dados desses arquivos auxilia na tomada de decisões para o planejamento de negócios e desenvolvimento[1][2].

A iTaaS Solution é uma empresa focada em entrega de conteúdo, e um dos seus maiores desafios de negócio era o custo com CDN (Content Delivery Network). Custos altos de CDN aumentam o preço final para os seus clientes, reduzem lucros e dificultam a entrada em mercados menores[1][2].

Após uma extensa pesquisa realizada por seus engenheiros de software e equipe financeira, eles obtiveram um excelente negócio com a empresa "MINHA CDN" e assinaram um contrato de um ano com eles[1][2].

A solução iTaaS já possui um sistema para gerar relatórios de faturamento a partir de logs, chamado "Agora". No entanto, ele utiliza um formato de log específico, diferente do formato utilizado pelo "MINHA CDN"[1][2].

A iTaaS Solution te contratou para desenvolver um sistema capaz de converter arquivos de log para o formato desejado. Neste momento, eles precisam converter do formato "MINHA CDN" para o formato "Agora"[1][2].

Este é um arquivo de log de exemplo no formato "MINHA CDN":

```
312|200|HIT|"GET /robots.txt HTTP/1.1"|100.2
101|200|MISS|"POST /myImages HTTP/1.1"|319.4
199|404|MISS|"GET /not-found HTTP/1.1"|142.9
312|200|INVALIDATE|"GET /robots.txt HTTP/1.1"|245.1
```

O exemplo acima deve gerar o seguinte log no formato 'Agora':

```
#Version: 1.0
#Date: 15/12/2017 23:01:06
#Fields: provider http-method status-code uri-path time-taken response-size cache-status
```

```
"MINHA CDN" GET 200 /robots.txt 100 312 HIT
"MINHA CDN" POST 200 /myImages 319 101 MISS
"MINHA CDN" GET 404 /not-found 143 199 MISS
"MINHA CDN" GET 200 /robots.txt 245 312 REFRESH_HIT
```

"MINHA CDN" irá gerar arquivos de log através de URLs específicas[1][2].

## Especificações da API

A especificação exige que você implemente uma API Rest com um conjunto de endpoints[1][2].

Os endpoints obrigatórios são:

1. Transformação de um formato de Log para outro:
   - Log de entrada pode vir:
     - Em uma url
     - Pode ser um identificador para um log que já foi salvo no banco de dados
   - Formato de saída pode variar (O usuário vai selecionar na requisição):
     - O resultado pode ser salvar em um arquivo no servidor e colocar retornar o path dele
     - O resultado pode ser a resposta da call com o resultado do log mudado

2. Buscar Logs Salvos

3. Buscar Logs Transformados no Backend:
   - Retorna o Log Original no formato "MINHA CDN" e a saída no formato "Agora"

4. Buscar Logs Salvos por Identificador

5. Buscar Logs Transformados por Identificador

6. Salvar Logs

Para esse teste considere que só existe o formato de entrada "MINHA CDN", e a saída no formato "Agora"[1][2].

## Instruções Adicionais

- Coloque toda a sua solução em um projeto único. Os testes unitários podem ficar em um projeto diferente[1][2].

- Um arquivo de log de exemplo que pode ser usado para teste está disponível em: https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt[1][2]

- Tenha cuidado, pois o que será analisado neste exercício não é apenas a saída correta do código, mas também as boas práticas de codificação, como POO, SOLID, testes unitários e mocks[1][2].

## Tecnologias Obrigatórias

- .Net Core 2.1
- Entity Framework Core
- SQL Server[1][2]
