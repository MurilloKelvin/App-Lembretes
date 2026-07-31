import { useState, useEffect } from 'react';
import axios from 'axios';
import { Trash2 } from 'lucide-react';
import styles from './App.module.css';

// URL da API local
const API_URL = 'http://localhost:5094/api/reminders';

export default function App() {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [date, setDate] = useState('');
  
  // Estado para guardar a lista real de lembretes vinda do banco
  const [reminders, setReminders] = useState([]);
  const today = new Date().toISOString().split('T')[0]; // Data atual no formato YYYY-MM-DD

  // 1. BUSCAR OS LEMBRETES (GET)
  const fetchReminders = async () => {
    try {
      const response = await axios.get(API_URL);
      setReminders(response.data);
    } catch (error) {
      console.error("Erro ao buscar lembretes:", error);
    }
  };

  // Executa a busca assim que a tela abre
  useEffect(() => {
    fetchReminders();
  }, []);

  // 2. CRIAR UM LEMBRETE (POST)
  const handleCreate = async (e) => {
    e.preventDefault(); // Evita que a página recarregue ao enviar o form
    
    try {
      await axios.post(API_URL, {
        name: name,
        description: description,
        // O C# espera "reminderDate" 
        reminderDate: date 
      });

      // Limpa os campos após salvar
      setName('');
      setDescription('');
      setDate('');
      
      // Busca a lista atualizada no banco
      fetchReminders();
    } catch (error) {
      if (error.response && error.response.status === 400) {
        // Tenta pegar a string direta, se não achar, tenta navegar no objeto de erros do C#, ou exibe uma mensagem padrão.
        const erroCsharp = error.response.data;
        const mensagemReal = typeof erroCsharp === 'string' ? erroCsharp : erroCsharp?.errors?.[Object.keys(erroCsharp.errors)[0]]?.[0] || "Erro de validação verifique os campos.";
        
        alert(mensagemReal);
      } else {
        console.error("Erro ao criar:", error);
      }
    }
  };

  // 3. DELETAR UM LEMBRETE (DELETE)
  const handleDelete = async (id) => {
    try {
      await axios.delete(`${API_URL}/${id}`);
      fetchReminders(); // Atualiza a tela após deletar
    } catch (error) {
      console.error("Erro ao deletar:", error);
    }
  };

  // --- LÓGICA DE AGRUPAMENTO  ---
  const groupedReminders = reminders.reduce((grupos, reminder) => {
    // Pega só a parte "YYYY-MM-DD" da data que volta do C#
    const dataFormatada = reminder.reminderDate.split('T')[0]; 
    if (!grupos[dataFormatada]) {
      grupos[dataFormatada] = [];
    }
    grupos[dataFormatada].push(reminder);
    return grupos;
  }, {});

  // Ordena as datas cronologicamente
  const sortedDates = Object.keys(groupedReminders).sort();

  // Função para transformar "YYYY-MM-DD" em "DD/MM/YYYY" 
  const formatDateToBR = (dateString) => {
    const [year, month, day] = dateString.split('-');
    return `${day}/${month}/${year}`;
  };

  return (
    <main className={styles.container}>
      {/* SEÇÃO DE CRIAÇÃO */}
      <section className={styles.formSection}>
        <h2>Novo lembrete</h2>
        
        {/*   onSubmit aqui */}
        <form className={styles.form} onSubmit={handleCreate}>
          <div className={styles.inputGroup}>
            <label htmlFor="name">Nome</label>
            <input
              id="name"
              type="text"
              placeholder="Nome do lembrete"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required // Validação HTML
            />
          </div>

          <div className={styles.inputGroup}>
            <label htmlFor="description">Descrição</label>
            <input
              id="description"
              type="text"
              placeholder="Detalhes adicionais"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            />
          </div>

          <div className={styles.inputGroup}>
            <label htmlFor="date">Data</label>
            <input
              id="date"
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
              min={today} // Impede datas passadas no calendario
              required // Validação HTML
            />
          </div>

          <button type="submit" className={styles.submitButton}>
            Criar
          </button>
        </form>
      </section>

      {/* SEÇÃO DA LISTA DINÂMICA */}
      <section className={styles.listSection}>
        <h2>Lista de lembretes</h2>
        
        {sortedDates.length === 0 ? (
          <p>Nenhum lembrete cadastrado.</p>
        ) : (
          sortedDates.map(dateKey => ( /* ordena as datas  */
            <div key={dateKey} className={styles.dateGroup}>
              {/* Data formatada como título do grupo */}
              <h3>{formatDateToBR(dateKey)}</h3>
              <ul className={styles.reminderList}>
                
                {/* Lembretes daquela data específica */}
                {groupedReminders[dateKey].map(reminder => (
                  <li key={reminder.id}>
                    <div className={`${styles.reminderInfo} ${dateKey < today ? styles.pastReminder : ''}`}> {/* Adiciona um risco a lembretes passados */}
                      <strong>{reminder.name}</strong>
                      {reminder.description && <span>{reminder.description}</span>}
                    </div>
                    {/* Botão de Excluir passando o ID (Guid) */}
                    <button 
                      type="button" 
                      className={styles.deleteButton} 
                      onClick={() => handleDelete(reminder.id)}
                      title="Excluir"
                    >
                      <Trash2 size={20} />
                    </button>
                  </li>
                ))}
                
              </ul>
            </div>
          ))
        )}
      </section>
    </main>
  );
}