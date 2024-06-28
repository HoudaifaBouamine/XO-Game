import Box from '../box/Box'
import './XO.css'

const XO = () => {
  return (
    <div className="xo">
        <Box value="x" />
        <Box value="o"/>
        <Box value="o"/>
        <Box value='x'/>
        <Box value='x'/>
        <Box value='x'/>
        <Box value='x'/>
        <Box value='x'/>
        <Box value='x'/>
    </div>
  )
}

export default XO