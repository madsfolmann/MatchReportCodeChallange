import csgoBackground from "../Resources/csgo_background.webp";
import Match from "./Modules/Match/Match";

const App = () => {
  return (
    <div
      className="w-screen h-screen relative bg-cover bg-center"
      style={{ backgroundImage: `url(${csgoBackground})` }}
    >
      <div
        className="absolute inset-0"
        style={{ backgroundColor: "#1e0a18", opacity: 0.9 }}
      ></div>

      <div className="relative flex w-full h-full">
        <Match />
      </div>
    </div>
  );
};

export default App;
