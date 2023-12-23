import './Box.css'

type Props ={
  value:string
}

const Box = ({value}:Props) => {
  return (
    <button className='box'>{value}</button>
  )
}

export default Box