Descrição
Arquivos de log podem revelar muito sobre o comportamento de um sistema em um
ambiente de produção. A extração de dados desses arquivos auxilia na tomada de decisões
para o planejamento de negócios e desenvolvimento.
A iTaaS Solution é uma empresa focada em entrega de conteúdo, e um dos seus maiores
desaos de negócio era o custo com CDN (Content Delivery Network). Custos altos de CDN
aumentam o preço nal para os seus clientes, reduzem lucros e dicultam a entrada em
mercados menores.
Após uma extensa pesquisa realizada por seus engenheiros de software e equipe
nanceira, eles obtiveram um excelente negócio com a empresa “MINHA CDN” e assinaram
um contrato de um ano com eles.
A solução iTaaS já possui um sistema para gerar relatórios de faturamento a partir de logs,
chamado “Agora”. No entanto, ele utiliza um formato de log especíco, diferente do formato
utilizado pelo "MINHA CDN".
A iTaaS Solution te contratou para desenvolver um sistema capaz de converter arquivos de
log para o formato desejado. Neste momento, eles precisam converter do formato “MINHA
CDN” para o formato “Agora”.
Este é um arquivo de log de exemplo no formato "MINHA CDN":
312|200|HIT|"GET /robots.txt HTTP/1.1"|100.2
101|200|MISS|"POST /myImages HTTP/1.1"|319.4
199|404|MISS|"GET /not-found HTTP/1.1"|142.9
312|200|INVALIDATE|"GET /robots.txt HTTP/1.1"|245.1
O exemplo acima deve gerar o seguinte log no formato 'Agora':
#Version: 1.0
#Date: 15/12/2017 23:01:06
#Fields: provider http-method status-code uri-path time-taken
response-size cache-status
1
"MINHA CDN" GET 200 /robots.txt 100 312 HIT
"MINHA CDN" POST 200 /myImages 319 101 MISS
"MINHA CDN" GET 404 /not-found 143 199 MISS
"MINHA CDN" GET 200 /robots.txt 245 312 REFRESH_HIT
“MINHA CDN” irá gerar arquivos de log através de URLs especícas.
A especicação exige que você implemente uma API Rest com um conjunto de endpoints.
Os endpoints obrigatórios são:
● Transformação de um formato de Log para outro;
○ Log de entrada pode vir:
■ Em uma url;
■ Pode ser um identicador para um log que já foi salvo no banco de
dados;
○ Formato de saída pode variar (O usuário vai selecionar na requisição):
■ O resultado pode ser salvar em um arquivo no servidor e colocar
retornar o path dele;
■ O resultado pode ser a resposta da call com o resultado do log
mudado;
● Buscar Logs Salvos;
● Buscar Logs Transformados no Backend;
○ Retorna o Log Original no formato "MINHA CDN" e a saída no formato
“Agora”;
● Buscar Logs Salvos por Identicador;
● Buscar Logs Transformados por Identicador;
● Salvar Logs;
Para esse teste considere que só existe o formato de entrada "MINHA CDN", e a saída no
formato "Agora".
Coloque toda a sua solução em um projeto único. Os testes unitários podem car em um
projeto diferente.
Um arquivo de log de exemplo que pode ser usado para teste está disponível aqui:
https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt
Tenha cuidado, pois o que será analisado neste exercício não é apenas a saída correta do
código, mas também as boas práticas de codicação, como POO, SOLID, testes unitários e
mocks.
Tecnologias obrigatórias a serem usadas:
2
● .Net Core 2.1;
● Entity Framework Core;
● SQL Server.
