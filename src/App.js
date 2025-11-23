import logo from './logo.svg';
import './App.css';
import AdminstratorLogin from './views/AdminstratorLogin'
function App() {
  // const [loggedIn, setLoggedIn] = useState(false);
  return (

    <div className="App">
      <AdminstratorLogin/>
    </div>
    // <>
    //   {!loggedIn ? (
    //     <AdminstratorLogin onLogin={() => setLoggedIn(true)} />
    //   ) : (
    //     <h1 style={{ textAlign: "center" }}>مدیر وارد شد ✔</h1>
    //   )}
    // </>
  );
}

export default App;
