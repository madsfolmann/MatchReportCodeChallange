export const RoundsTableCell = ({
  value,
  rowCounter,
  className,
  borderStyle,
}: {
  value?: any;
  rowCounter?: boolean;
  className?: string;
  borderStyle: string;
}) => {
  return (
    <div
      className={`flex items-center ${borderStyle} ${
        rowCounter ? "py-1 px-1 justify-center" : "py-2 px-3"
      } ${className ?? ""}`}
    >
      {value}
    </div>
  );
};
