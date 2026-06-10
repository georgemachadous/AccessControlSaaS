import { Outlet } from 'react-router-dom';
import Sidebar from '../components/Sidebar';
import Header from '../components/Header';

export default function Layout() {
  return (
    <div style={{ display: 'flex', height: '100vh', overflow: 'hidden' }}>
      <Sidebar />
      <div className="main-wrapper">
        <Header title="" />
        <main className="main-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
