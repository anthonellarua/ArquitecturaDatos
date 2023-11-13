const { Pool } = require('pg');

const pool = new Pool({
  user: 'postgres',
  host: 'localhost',
  database: 'usuarios',
  password: 'hermana1',
  port: 5432,
});

const consulta = 'SELECT * FROM usuarios_calificaciones';

pool.query(consulta, (error, resultados) => {
  if (error) {
    console.error('Error al ejecutar la consulta', error);
  } else {
    console.log('Datos obtenidos de PostgreSQL:', resultados.rows);
  }

  pool.end();
});
